import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopMenuLayoutComponent } from './top-menu-layout.component';

describe('TopMenuLayoutComponent', () => {
  let component: TopMenuLayoutComponent;
  let fixture: ComponentFixture<TopMenuLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TopMenuLayoutComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TopMenuLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
