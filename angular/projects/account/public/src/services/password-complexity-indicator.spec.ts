import { TestBed } from '@angular/core/testing';
import { PasswordComplexityIndicatorService } from './password-complexity-indicator.service'


describe('InternetConnectionService', () => {
  let service: PasswordComplexityIndicatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PasswordComplexityIndicatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('validatePassword should return value', () => {
    expect(service.validatePassword('')).toBeTruthy();
  });

  test.each([['bgColor','string'],["text",'string'],["width",'number']])('check validatePassword return values %p property type must be %p',(property,type)=>{
    expect(typeof service.validatePassword('1q2w3E*')[property]).toEqual(type)
  })

  it('progress bars colors,texts and test count should be equal', () => {
    const colorsLength = service.colors.length
    const textLength = service.texts.length
    const testLength = Object.keys(service.requirements).length
    expect(colorsLength - testLength).toBe(0)
    expect(colorsLength - textLength).toBe(0)
  });
});
