export { default as Router } from "./Router.svelte";
export { routeParams, currentURL } from "./stores.js";
export { link } from "./link.js";
export { push } from "./push.js";

export function redirect(to) {
  return { redirect: to };
}
