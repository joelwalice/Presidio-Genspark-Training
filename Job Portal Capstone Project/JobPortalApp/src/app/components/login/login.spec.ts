import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Login } from './login';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { JobSeekerService } from '../../services/user/job-seeker';
import { UserLoginService } from '../../services/user/UserLoginService';

describe('JobSeeker Login', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Login, HttpClientTestingModule],
      providers: [UserLoginService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a login method', () => {
    expect(component.loginForm).toBeDefined();
  });
});
