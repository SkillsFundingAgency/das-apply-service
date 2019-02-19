using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    
    public class UploadController : Controller
    {
        private readonly IStorageService _storageService;

        public UploadController(IStorageService storageService)
        {
            _storageService = storageService;
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Chunks()
        {
            var a = HttpContext.Request;

            var formItems = await a.ReadFormAsync();


            var configuration = new ResumableConfiguration()
            {
                Chunks = int.Parse(formItems["resumableTotalChunks"][0]),
                ChunkNumber = int.Parse(formItems["resumableChunkNumber"][0]),
                Identifier = formItems["resumableIdentifier"][0],
                ApplicationId = formItems["applicationId"][0],
                SequenceId = int.Parse(formItems["sequenceId"][0]),
                SectionId = int.Parse(formItems["sectionId"][0]),
                PageId = formItems["page"][0],
                QuestionId = formItems["questionId"][0],
                FileName = formItems["resumableFilename"][0],
                Type = formItems["resumableType"][0]
            };
            
            SaveFile(formItems.Files[0], configuration);
            
            await TryAssembleFile(configuration);
            
            return Ok();
        }

        private void SaveFile(IFormFile fileChunk, ResumableConfiguration configuration)
        {
            var openReadStream = fileChunk.OpenReadStream();

            var fileStream = new MemoryStream();
            openReadStream.CopyTo(fileStream);

            fileStream.Position = 0;
            
            _storageService.Store(configuration.ApplicationId, configuration.SequenceId,
                configuration.SectionId, configuration.PageId, configuration.QuestionId,
                $"{configuration.Identifier}_{configuration.ChunkNumber}", fileStream, fileChunk.ContentType).Wait();
        }
        
        private async Task TryAssembleFile(ResumableConfiguration configuration)
        {
            if (AllChunksAreHere(configuration))
            {
                // Create a single file
                var assembledFileStream = await ConsolidateFile(configuration);

                // Rename consolidated with original name of upload
                await _storageService.Store(configuration.ApplicationId, configuration.SequenceId,
                    configuration.SectionId, configuration.PageId, configuration.QuestionId,
                configuration.FileName, assembledFileStream, configuration.Type);

                // Delete chunk files
                await DeleteChunks(configuration);
            }
        }

        private async Task DeleteChunks(ResumableConfiguration configuration)
        {
            for (int chunkNumber = 1; chunkNumber <= configuration.Chunks; chunkNumber++)
            {
                var chunkFileName = $"{configuration.Identifier}_{chunkNumber}";
                await _storageService.Delete(Guid.Parse(configuration.ApplicationId), configuration.SequenceId, configuration.SectionId, configuration.PageId, configuration.QuestionId, chunkFileName);
            }
        }

        private async Task<Stream> ConsolidateFile(ResumableConfiguration configuration)
        {
            // create destination memory stream
            var dest = new MemoryStream();

            for (int chunkNumber = 1; chunkNumber <= configuration.Chunks; chunkNumber++)
            {
                var chunkFileName = $"{configuration.Identifier}_{chunkNumber}";
                var chunk = await _storageService.Retrieve(configuration.ApplicationId, configuration.SequenceId, configuration.SectionId, configuration.PageId, configuration.QuestionId, chunkFileName);
                
                var ms = new MemoryStream();
                
                chunk.Item2.CopyTo(ms);
                
                dest.Write(ms.ToArray(), 0, (int)chunk.Item2.Length);
            }

            dest.Position = 0;
            
            return dest;

            // get each file from storage in turn
            // append stream to destination stream.
            // set destination position to 0
            // return stream
        }

        private bool AllChunksAreHere(ResumableConfiguration configuration)
        {
            for (int chunkNumber = 1; chunkNumber <= configuration.Chunks; chunkNumber++)
                if (!ChunkIsHere(chunkNumber, configuration)) return false;
            return true;
        }
        
        private bool ChunkIsHere(int chunkNumber, ResumableConfiguration configuration)
        {
            string fileName = $"{configuration.Identifier}_{chunkNumber}";//   GetChunkFileName(chunkNumber, identifier);
            return _storageService.Exists(configuration.ApplicationId, configuration.SequenceId,
                configuration.SectionId, configuration.PageId, configuration.QuestionId,
                fileName).Result;
        }

    }

    class ResumableConfiguration
    {
        public int Chunks { get; set; }
        public string Identifier { get; set; }
        public string ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string QuestionId { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public int ChunkNumber { get; set; }
    }
}