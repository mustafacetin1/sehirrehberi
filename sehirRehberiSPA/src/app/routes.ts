import { CityComponent } from "./city/city.component";
import { ValueComponent } from "./value/value.component";
import {Routes} from "@angular/router";
import { RegisterComponent } from "./register/register.component";
import { CityDetailComponent } from "./city/city-detail/city-detail.component";
import { CityAddComponent } from "./city/city-add/city-add.component";

export const appRoutes: Routes=[
    { path: "value", component: ValueComponent },
   
    {path:"cityadd", component:CityAddComponent},
    { path: "city", component: CityComponent },
    { path: "register", component: RegisterComponent },
    { path: "cityDetail/:cityId", component: CityDetailComponent },
    { path: "**", redirectTo: "city", pathMatch: "full" }
];

