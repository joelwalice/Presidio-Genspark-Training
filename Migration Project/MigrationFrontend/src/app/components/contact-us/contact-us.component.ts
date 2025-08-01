
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ContactUsService } from '../../services/contact-us.service';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  imports: [ReactiveFormsModule]
})
export class ContactUsComponent {
  fb = inject(FormBuilder);
  contactUsService = inject(ContactUsService);

  contactForm = this.fb.group({
    name: '',
    email: '',
    message: ''
  });

  onSubmit() {
    const { name, email, message } = this.contactForm.value;
    this.contactUsService.send({
      name: name ?? '',
      email: email ?? '',
      message: message ?? ''
    }).subscribe(() => {
      this.contactForm.reset();
    });
  }
}
