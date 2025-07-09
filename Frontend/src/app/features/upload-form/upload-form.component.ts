import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-upload-form',
  imports: [ReactiveFormsModule],
  templateUrl: './upload-form.component.html',
  styleUrl: './upload-form.component.scss'
})
export class UploadFormComponent {
  private formBuilder = inject(FormBuilder);

  previewSrc = '#';

  uploadedFile?: File = undefined;

  isAuthFormShown: Boolean = false;

  uploadForm = this.formBuilder.group({
    name: ['', Validators.required],
    // File.prototype gives Error DOMException, 
    // but it's the only way it works
    file: [File.prototype , Validators.required],
    tags: ['', Validators.required],
  });

  onSubmit() {
    let uploadFormData = this.uploadForm.value;
    let formData = new FormData();
    let tags = uploadFormData.tags!.split(' ');

    formData.append("File", this.uploadedFile!);
    formData.append("Name", uploadFormData.name!);
    formData.append("Tags", JSON.stringify(tags));
  }

  UpdatePreview(file: File) {
    this.uploadedFile = file;
    this.previewSrc = URL.createObjectURL(file);
  }

  onImagePicked(event: Event) {
    const uploadedFile = (event.target as HTMLInputElement).files![0];
    this.UpdatePreview(uploadedFile);
  }
}
