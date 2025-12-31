using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.FileManagement.Directories;

namespace Volo.FileManagement.Web.Pages.FileManagement.Directory;

public class MoveModalModel : FileManagementPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public MoveDirectoryInput MoveDirectoryInput { get; set; }

    protected IDirectoryDescriptorAppService DirectoryDescriptorAppService { get; }

    public MoveModalModel(IDirectoryDescriptorAppService directoryDescriptorAppService)
    {
        DirectoryDescriptorAppService = directoryDescriptorAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var directoryDescriptorDto = await DirectoryDescriptorAppService.GetAsync(Id);
        MoveDirectoryInput = new MoveDirectoryInput
        {
            Id = directoryDescriptorDto.Id,
            NewParentId = null,
            ConcurrencyStamp = directoryDescriptorDto.ConcurrencyStamp
        };

    }

    public virtual async Task OnPostAsync()
    {
        await DirectoryDescriptorAppService.MoveAsync(MoveDirectoryInput);
    }
}
