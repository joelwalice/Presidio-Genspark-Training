import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilePage } from './profile-page';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { JobSeekerService } from '../../services/user/job-seeker';

describe('JobSeeker Profile Page', () => {
  let component: ProfilePage;
  let fixture: ComponentFixture<ProfilePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfilePage, HttpClientTestingModule],
      providers: [JobSeekerService]

    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfilePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
