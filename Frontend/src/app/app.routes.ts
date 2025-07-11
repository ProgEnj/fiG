import { Routes } from '@angular/router';
import { UploadFormComponent } from './features/upload-form/upload-form.component';
import { MainPageComponent } from './features/main-page/main-page.component';
import { RefreshComponent } from './core/components/refresh/refresh.component';

export const routes: Routes = [
    {
        path: '',
        component: MainPageComponent
    },
    {
        path: 'upload',
        component: UploadFormComponent
    },
    {
        path: 'refresh',
        component: RefreshComponent
    },
];
