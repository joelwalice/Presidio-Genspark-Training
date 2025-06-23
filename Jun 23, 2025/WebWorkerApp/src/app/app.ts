import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FileUploadComponent } from "./file-upload-component/file-upload-component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FileUploadComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'WebWorkerApp';
}
