import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Navbar } from './navbar';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { JobSeekerService } from '../../services/user/job-seeker';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('JobSeeker Navbar', () => {
  let component: Navbar;
  let fixture: ComponentFixture<Navbar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Navbar, HttpClientTestingModule],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            snapshot: {
              paramMap: {
                get: (key: string) => null
              }
            }
          }
        }
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(Navbar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
