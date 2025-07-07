import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecruiterApplicants } from './recruiter-applicants';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('RecruiterApplicants', () => {
  let component: RecruiterApplicants;
  let fixture: ComponentFixture<RecruiterApplicants>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RecruiterApplicants],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ jobId: '42' }),
            snapshot: {
              paramMap: {
                get: (jobId: string) => '42',
              },
            },
          },
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruiterApplicants);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the RecruiterApplicants component', () => {
    expect(component).toBeTruthy();
  });

  it('should receive jobId from route', () => {
    // Example if you're storing jobId inside component
    expect((component as any).jobId).toBe('42');
  });
});
