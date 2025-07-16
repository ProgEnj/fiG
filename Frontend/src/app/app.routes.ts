import { Routes } from '@angular/router';
import { UploadFormComponent } from './features/upload-form/upload-form.component';
import { MainPageComponent } from './features/main-page/main-page.component';
import { RefreshComponent } from './core/components/refresh/refresh.component';
import { uploadGuard } from './core/guards/upload.guard';
import { LoginFormComponent } from './features/login-form/login-form.component';
import { AuthenticationFormComponent } from './features/authentication-form/authentication-form.component';
import { GifPageComponent } from './features/gif-page/gif-page.component';
import { NotFoundPageComponent } from './core/components/not-found-page/not-found-page.component';

export const routes: Routes = [
    {
        path: '',
        component: MainPageComponent,
        children: [
            { path: 'login', component: LoginFormComponent },
            { path: 'signup', component: AuthenticationFormComponent }
        ]
    },
    {
        path: 'view/:id',
        component: GifPageComponent,
        children: [
            { path: 'login', component: LoginFormComponent },
            { path: 'signup', component: AuthenticationFormComponent }
        ]
    },
    {
        path: 'upload',
        component: UploadFormComponent,
        canActivate: [uploadGuard]
    },
    {
        path: 'refresh',
        component: RefreshComponent
    },
    {
        path: '**',
        component: NotFoundPageComponent
    },
];
