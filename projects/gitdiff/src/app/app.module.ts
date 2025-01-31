import {DoBootstrap, Injector, NgModule, ProviderToken} from '@angular/core';
import {BrowserModule } from '@angular/platform-browser';
import {bootstrapCrtModule, CrtModule} from '@creatio-devkit/common';
import {GitDiffComponent } from './view-elements/gitdiff/gitdiff.component';
import {createCustomElement} from "@angular/elements";

@CrtModule({
  /* Specify that InputComponent is a view element. */
  viewElements: [GitDiffComponent],
})
@NgModule({
  declarations: [
    GitDiffComponent
  ],
  imports: [BrowserModule],
  providers: [],
})
export class AppModule implements DoBootstrap {
  constructor(private _injector: Injector) {}

  ngDoBootstrap(): void {
    /* Register InputComponent as an Angular Element. */
    const cmp = createCustomElement(GitDiffComponent, {
      injector: this._injector,
    });
    customElements.define("atf-gitdiff", cmp);
    /* Bootstrap CrtModule definitions. */
    bootstrapCrtModule('gitdiff', AppModule, {
      resolveDependency: (token) => this._injector.get(<ProviderToken<unknown>>token)
    });
  }
}