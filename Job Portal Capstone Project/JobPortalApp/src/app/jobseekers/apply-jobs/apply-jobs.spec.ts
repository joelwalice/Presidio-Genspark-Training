import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyJobs } from './apply-jobs';
import { JobSeekerService } from '../../services/user/job-seeker';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('ApplyJobs', () => {
  let component: ApplyJobs;
  let fixture: ComponentFixture<ApplyJobs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplyJobs, HttpClientTestingModule],
      providers: [
              {
                provide: ActivatedRoute,
                useValue: {
                  params: of({ id: '42' }), 
                  snapshot: {
                    paramMap: {
                      get: (id: string) => '42', 
                    },
                  },
                },
              }
            ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplyJobs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
