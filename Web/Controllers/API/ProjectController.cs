using Businness.Interface.API;
using Entities.DTO;
using Entities.Entity;
using Entities.Enums;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.API
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IProjectImageService _projectImageService;

        public ProjectController(IProjectService projectService, IProjectImageService projectImageService)
        {
            _projectService = projectService;
            _projectImageService = projectImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAll();
            return Ok(new BaseResponse<List<Project>>() { Success = true, Data = projects.ToList() });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var project = _projectService.GetById(id);
            if (project == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID'ye ait herhangi bir proje bulunamadı. ID: {id}" });
            return Ok(new BaseResponse<Project>() { Success = true, Data = project });
        }

        [HttpGet]
        public IActionResult GetByProjectType(SolutionTypes projectType)
        {
            var projects = _projectService.GetBySolutionType(projectType).ToList();
            if (projects.Count == 0)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"{projectType} Proje tipine ait herhangi bir proje bulunamadı." });
            return Ok(new BaseResponse<List<Project>>() { Success = true, Data = projects });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetByCompletedStatus(bool isCompleted)
        {
            var projects = _projectService.GetByCompleteStatus(isCompleted).ToList();
            if (projects.Count == 0)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"{(isCompleted ? "Completed" : "On Going")} Statüsüne ait herhangi bir proje bulunamadı." });
            return Ok(new BaseResponse<List<Project>>() { Success = true, Data = projects });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProjectDTO model)
        {

            if (model.MainImage == null || model.MainImage.Length == 0)
            {
                return BadRequest(new BaseResponse<string>() { Success = false, Data = $"Ana Görsel Zorunludur." });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var project = new Project
                    {

                        Name = model.Name,
                        ProjectType = model.ProjectType,
                        IsCompleted = model.IsCompleted,
                        ClientName = model.ClientName,
                        Year = model.Year,
                        ArchitectName = model.ArchitectName,
                        Location = model.Location
                    };
                    var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                    if (!resp.Success)
                    {
                        return StatusCode(500, resp);

                    }
                    string mainImagePath = resp.Data;
                    project.MainImagePath = mainImagePath;

                    await _projectService.CreateAsync(project);

                    if (model.AdditionalImages != null && model.AdditionalImages.Any())
                    {
                        string imagePath = "";
                        List<string> savedFiles = new List<string>();
                        var projectImages = new List<ProjectImage>();
                        foreach (var file in model.AdditionalImages)
                        {
                            resp = await SaveFileAsync(file, UploadRoutes.Images, DocumentTypes.Image);
                            if (!resp.Success)
                            {
                                foreach (var savedFile in savedFiles)
                                {
                                    DeleteFile(savedFile);
                                }
                                return StatusCode(500, resp);
                            }
                            imagePath = resp.Data;
                            projectImages.Add(new ProjectImage
                            {
                                ProjectId = project.Id,
                                ImagePath = imagePath,
                                AltName = file.FileName,
                                Status = true,
                                CreatedDate = DateTime.UtcNow,
                                UpdatedDate = DateTime.UtcNow
                            });
                            savedFiles.Add(imagePath);
                        }

                        await _projectImageService.AddRange(projectImages);

                    }

                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Proje başarıyla eklendi. Project_Id: {project.Id}" });
                }
                else
                {
                    return BadRequest(new BaseResponse<string>() { Success = false, Data = $"Model Hatası: {ModelState}" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ProjectDTO model)
        {
            var project = _projectService.GetById(id);
            if (project == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile proje bulunamadı. ID: {id}" });
            }
            try
            {

                if (ModelState.IsValid)
                {
                    project.Name = model.Name;
                    project.ProjectType = model.ProjectType;
                    project.IsCompleted = model.IsCompleted;
                    project.ClientName = model.ClientName;
                    project.Year = model.Year;
                    project.ArchitectName = model.ArchitectName;
                    project.Location = model.Location;

                    if (model.MainImage != null)
                    {
                        if (!string.IsNullOrEmpty(project.MainImagePath))
                        {
                            DeleteFile(project.MainImagePath);
                        }
                        var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                        if (!resp.Success)
                        {
                            return StatusCode(500, resp);
                        }
                        project.MainImagePath = resp.Data;
                    }
                    if (model.AdditionalImages != null && model.AdditionalImages.Count > 0)
                    {

                        foreach (var oldImage in project.ProjectImage)
                        {
                            DeleteFile(oldImage.ImagePath);
                            await _projectImageService.DeleteAllByProjectId(oldImage.ProjectId);
                        }
                        string savedPath = "";
                        List<string> SavedFiles = new List<string>();
                        foreach (var image in model.AdditionalImages)
                        {
                            var resp = await SaveFileAsync(image, UploadRoutes.Images, DocumentTypes.Image);
                            if (!resp.Success)
                            {
                                foreach (var savedFile in SavedFiles)
                                {
                                    DeleteFile(savedFile);
                                }
                                return StatusCode(500, resp);
                            }
                            savedPath = resp.Data;
                            SavedFiles.Add(savedPath);
                            project.ProjectImage.Add(new ProjectImage
                            {
                                ImagePath = savedPath,
                                AltName = image.FileName,
                                Status = true,
                                CreatedDate = DateTime.UtcNow,
                                UpdatedDate = DateTime.UtcNow
                            });
                        }
                    }
                    await _projectService.Update(project);
                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Proje başarıyla güncellendi. Project_Id: {project.Id}" });
                }
                else
                {
                    return BadRequest(new BaseResponse<string>() { Success = true, Data = $"Model Hatası: {ModelState}" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var project = _projectService.GetById(id);
            if (project == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile proje bulunamadı. ID: {id}" });
            }
            try
            {
                if (!string.IsNullOrEmpty(project.MainImagePath))
                {
                    DeleteFile(project.MainImagePath);
                }
                if (project.ProjectImage.Count > 0)
                {
                    foreach (var oldImage in project.ProjectImage)
                    {
                        DeleteFile(oldImage.ImagePath);
                    }
                    await _projectImageService.DeleteAllByProjectId(id);
                }
                await _projectService.DeleteAsync(id);
                return Ok(new BaseResponse<string>() { Success = true, Data = $"Proje ve projeye ait tüm görseller başarıyla silindi." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });

            }
        }
    }
}
