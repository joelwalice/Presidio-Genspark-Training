import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterNavbar } from './recruiter-navbar';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RecruiterService } from '../../services/recruiter/recruiter-service';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('Recruiter Navbar', () => {
  let component: RecruiterNavbar;
  let fixture: ComponentFixture<RecruiterNavbar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecruiterNavbar, HttpClientTestingModule],
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

    fixture = TestBed.createComponent(RecruiterNavbar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
