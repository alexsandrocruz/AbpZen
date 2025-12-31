import {
  authGuard,
  permissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LanguageTextsComponent } from './components/language-texts/language-texts.component';
import { LanguagesComponent } from './components/languages/languages.component';
import { eLanguageManagementComponents } from './enums/components';
import { languageManagementGuard } from './guards/language-mangement.guard';
import { languageManagementExtensionsResolver } from './resolvers';

const routes: Routes = [
  { path: '', redirectTo: 'languages', pathMatch: 'full' },
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [
      authGuard,
      permissionGuard,
      languageManagementGuard,
    ],
    resolve: [languageManagementExtensionsResolver],
    children: [
      {
        path: 'languages',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'LanguageManagement.Languages',
          replaceableComponent: {
            key: eLanguageManagementComponents.Languages,
            defaultComponent: LanguagesComponent,
          } as ReplaceableComponents.RouteData<LanguagesComponent>,
        },
        title: "LanguageManagement::Languages"
      },
      {
        path: 'texts',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'LanguageManagement.LanguageTexts',
          replaceableComponent: {
            key: eLanguageManagementComponents.LanguageTexts,
            defaultComponent: LanguageTextsComponent,
          } as ReplaceableComponents.RouteData<LanguageTextsComponent>,
        },
        title: "LanguageManagement::LanguageTexts"
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LanguageManagementRoutingModule {}
