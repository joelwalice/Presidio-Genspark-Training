import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterRegister } from './register';
import { RecruiterRegisterService } from '../../services/recruiter/recruiter-register-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('Recruiter Register', () => {
  let component: RecruiterRegister;
  let fixture: ComponentFixture<RecruiterRegister>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterRegister, HttpClientTestingModule],
      providers: [RecruiterRegisterService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterRegister);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
