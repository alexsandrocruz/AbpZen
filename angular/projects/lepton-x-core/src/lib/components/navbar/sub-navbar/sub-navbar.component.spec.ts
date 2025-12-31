import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubNavbarComponent } from './sub-navbar.component';

describe('SubNavbarComponent', () => {
  let component: SubNavbarComponent;
  let fixture: ComponentFixture<SubNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubNavbarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
