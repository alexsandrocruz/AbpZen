import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { permissionGuard, authGuard } from '@abp/ng.core';
import { FileManagementComponent } from './file-management.component';
import { fileManagementGuard } from './guards/file-management.guard';
import { fileManagementExtensionsResolver } from './resolvers';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    canActivate: [authGuard, permissionGuard, fileManagementGuard],
    resolve: [fileManagementExtensionsResolver],
    component: FileManagementComponent,
    title: 'FileManagement::Menu:FileManagement',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FileManagementRoutingModule {}
