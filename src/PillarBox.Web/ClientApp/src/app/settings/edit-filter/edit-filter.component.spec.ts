import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFilterComponent } from './edit-filter.component';

describe('EditFilterComponent', () => {
  let component: EditFilterComponent;
  let fixture: ComponentFixture<EditFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
