import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { FormsModule } from '@angular/forms'; 
import { VideoService } from '../../services/video';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-video-upload',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './video-upload.html',
  styleUrl: './video-upload.css'
})
export class VideoUpload {
  title: string = '';
  description: string = '';
  selectedFile: File | null = null;
  uploading = false;
  uploadSuccess = false;
  uploadError: string | null = null;

  constructor(private videoService: VideoService, private router: Router) { }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.uploadError = null; 
    } else {
      this.selectedFile = null;
    }
  }

  onSubmit(): void {
    if (!this.title || !this.selectedFile) {
      this.uploadError = 'Title and video file are required.';
      return;
    }

    this.uploading = true;
    this.uploadSuccess = false;
    this.uploadError = null;

    this.videoService.uploadVideo(this.title, this.description, this.selectedFile).subscribe({
      next: (response) => {
        console.log('Upload successful:', response);
        this.uploadSuccess = true;
        this.uploading = false;
        
        this.title = '';
        this.description = '';
        this.selectedFile = null;
        (document.getElementById('videoFile') as HTMLInputElement).value = ''; 
        setTimeout(() => {
          this.router.navigate(['/videos']); 
        }, 2000);
      },
      error: (err) => {
        console.error('Upload failed:', err);
        this.uploadError = 'Video upload failed. Please try again.';
        this.uploading = false;
      }
    });
  }
}