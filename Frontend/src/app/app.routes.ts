import { Routes } from '@angular/router';
import { UploadFormComponent } from './upload-form/upload-form.component';
import { MainPageComponent } from './main-page/main-page.component';

export const routes: Routes = [
    {
        path: '',
        component: MainPageComponent
    },
    {
        path: 'upload',
        component: UploadFormComponent
    },
];
