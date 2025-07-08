import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Explore } from './explore';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { JobSeekerService } from '../../services/user/job-seeker';

describe('JobSeeker Explore', () => {
  let component: Explore;
  let fixture: ComponentFixture<Explore>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Explore, HttpClientTestingModule],
      providers: [JobSeekerService], 
    }).compileComponents();

    fixture = TestBed.createComponent(Explore);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
