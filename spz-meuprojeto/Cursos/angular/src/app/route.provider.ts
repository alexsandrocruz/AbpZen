import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/dashboard',
        name: '::Menu:Dashboard',
        iconClass: 'fas fa-chart-line',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'Sapienza.Cursos.Dashboard.Host || LeptonX.Dashboard.Tenant',
      },
      {
        path: '/alunos',
        name: '::Menu:Alunos',
        iconClass: 'fas fa-users',
        order: 3,
        layout: eLayoutType.application,
        requiredPolicy: 'Sapienza.Cursos.Aluno',
      },
      {
        path: '/cursos',
        name: '::Menu:Cursos',
        iconClass: 'fas fa-book',
        order: 4,
        layout: eLayoutType.application,
        requiredPolicy: 'Sapienza.Cursos.Curso',
      },
      {
        path: '/turmas',
        name: '::Menu:Turmas',
        iconClass: 'fas fa-graduation-cap',
        order: 5,
        layout: eLayoutType.application,
        requiredPolicy: 'Sapienza.Cursos.Turma',
      },
      // <ZenCode-Menu-Marker>
    ]);
  };
}
