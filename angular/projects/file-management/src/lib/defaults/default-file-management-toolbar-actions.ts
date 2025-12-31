import { ToolbarAction } from '@abp/ng.components/extensible';
import { DirectoryContentDto } from '@volo/abp.ng.file-management/proxy';

export const DEFAULT_FILE_MANAGEMENT_DIRECTORY_CONTENT_TOOLBAR_ACTIONS = ToolbarAction.createMany<
  DirectoryContentDto[]
>([]);
