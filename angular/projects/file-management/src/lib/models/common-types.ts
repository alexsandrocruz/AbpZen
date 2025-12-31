import { DirectoryContentDto } from '@volo/abp.ng.file-management/proxy';

export type FolderInfo = Pick<DirectoryContentDto, 'name' | 'id'>;
export type FileInfo = FolderInfo;
