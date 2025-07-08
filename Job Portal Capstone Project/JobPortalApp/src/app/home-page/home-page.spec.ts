import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HomePage } from './home-page';
import { ActivatedRoute } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { Router } from '@angular/router';

describe('Opening HomePage', () => {
  let component: HomePage;
  let fixture: ComponentFixture<HomePage>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    routerSpy = jasmine.createSpyObj('Router', ['navigateByUrl']);

    await TestBed.configureTestingModule({
      imports: [HomePage, HttpClientTestingModule],
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
        },
        {
          provide: Router,
          useValue: routerSpy
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HomePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should redirect to /jobseekers if token exists and role is JobSeeker', () => {
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      if (key === 'JwtToken') return 'mock-token';
      if (key === 'role') return 'JobSeeker';
      return null;
    });

    component.ngOnInit();

    expect(routerSpy.navigateByUrl).toHaveBeenCalledWith('/jobseekers');
  });

  it('should redirect to /recruiters/home if token exists and role is Recruiter', () => {
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      if (key === 'JwtToken') return 'mock-token';
      if (key === 'role') return 'Recruiter';
      return null;
    });

    component.ngOnInit();

    expect(routerSpy.navigateByUrl).toHaveBeenCalledWith('/recruiters/home');
  });
});
