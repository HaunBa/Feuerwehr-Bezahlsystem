import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm = this.fb.group({
    email:[null, Validators.compose(Validators.required, Validators.email)],
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    username: [null, Validators.required],
    password: [null, Validators.required],
    repPassword: [null, Validators.required]
  })

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }

}
