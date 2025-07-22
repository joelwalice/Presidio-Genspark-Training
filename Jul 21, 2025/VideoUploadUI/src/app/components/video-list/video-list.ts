import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoService } from '../../services/video';
import { Video } from '../../models/VideoUpload';

@Component({
  selector: 'app-video-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './video-list.html',
  styleUrl: './video-list.css'
})
export class VideoListComponent implements OnInit {
  videos: Video[] = [];
  loading = true;
  error: string | null = null;

  constructor(private videoService: VideoService) { }

  ngOnInit(): void {
    this.getVideos();
  }

  getVideos(): void {
    this.loading = true;
    this.error = null;
    this.videoService.getVideos().subscribe({
      next: (data) => {
        this.videos = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching videos:', err);
        this.error = 'Failed to load videos. Please try again later.';
        this.loading = false;
      }
    });
  }
}