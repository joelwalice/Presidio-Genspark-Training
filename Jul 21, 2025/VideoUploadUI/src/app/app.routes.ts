import { Routes } from '@angular/router';
import { VideoListComponent } from './components/video-list/video-list';
import { VideoUpload } from './components/video-upload/video-upload';

export const routes: Routes = [
  { path: '', redirectTo: '/videos', pathMatch: 'full' }, 
  { path: 'videos', component: VideoListComponent },
  { path: 'upload', component: VideoUpload },
  { path: '**', redirectTo: '/videos' } 
];