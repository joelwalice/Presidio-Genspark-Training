import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterLogin } from './login';
import { RecruiterLoginService } from '../../services/recruiter/recruiter-login-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('Login', () => {
  let component: RecruiterLogin;
  let fixture: ComponentFixture<RecruiterLogin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterLogin, HttpClientTestingModule],
      providers: [RecruiterLoginService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterLogin);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
