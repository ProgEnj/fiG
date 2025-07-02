import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderProfileDisplayComponent } from './header-profile-display.component';

describe('HeaderProfileDisplayComponent', () => {
  let component: HeaderProfileDisplayComponent;
  let fixture: ComponentFixture<HeaderProfileDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderProfileDisplayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderProfileDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
