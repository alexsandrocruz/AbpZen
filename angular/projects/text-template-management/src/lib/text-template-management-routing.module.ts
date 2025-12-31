import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InlineTemplateContentComponent } from './components/inline-template-content/inline-template-content.component';
import { TemplateContentsComponent } from './components/template-contents/template-contents.component';
import { TextTemplatesComponent } from './components/text-templates/text-templates.component';
import { eTextTemplateManagementComponents } from './enums/components';
import { textTemplateManagementGuard } from './guards/text-template-mangement.guard';
import { textTemplateManagementExtensionsResolver } from './resolvers/extensions.resolver';

const routes: Routes = [
  { path: '', redirectTo: 'text-templates', pathMatch: 'full' },
  {
    path: 'text-templates',
    component: RouterOutletComponent,
    canActivate: [authGuard, permissionGuard, textTemplateManagementGuard],
    resolve: [textTemplateManagementExtensionsResolver],
    children: [
      {
        path: '',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'TextTemplateManagement.TextTemplates',
          replaceableComponent: {
            key: eTextTemplateManagementComponents.TextTemplates,
            defaultComponent: TextTemplatesComponent,
          } as ReplaceableComponents.RouteData<TextTemplatesComponent>,
        },
        title: 'TextTemplateManagement::Menu:TextTemplates',
      },
      {
        path: 'contents',
        component: RouterOutletComponent,
        canActivate: [permissionGuard],
        data: { requiredPolicy: 'TextTemplateManagement.TextTemplates.EditContents' },
        children: [
          {
            path: 'inline/:name',
            component: ReplaceableRouteContainerComponent,
            data: {
              replaceableComponent: {
                key: eTextTemplateManagementComponents.InlineTemplateContent,
                defaultComponent: InlineTemplateContentComponent,
              } as ReplaceableComponents.RouteData<InlineTemplateContentComponent>,
            },
          },
          {
            path: ':name',
            component: ReplaceableRouteContainerComponent,
            data: {
              replaceableComponent: {
                key: eTextTemplateManagementComponents.TemplateContents,
                defaultComponent: TemplateContentsComponent,
              } as ReplaceableComponents.RouteData<TemplateContentsComponent>,
            },
          },
        ],
        title: 'TextTemplateManagement::Contents',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TextTemplateManagementRoutingModule {}
