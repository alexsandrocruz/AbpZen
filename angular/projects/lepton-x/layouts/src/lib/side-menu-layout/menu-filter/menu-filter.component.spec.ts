import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuFilterComponent } from './menu-filter.component';

describe('MenuFilterComponent', () => {
  let component: MenuFilterComponent;
  let fixture: ComponentFixture<MenuFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MenuFilterComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
