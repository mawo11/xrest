<script>
	import { _ } from '../services/localization/i18n'
	import { setCallbackToFireWhenUnauthorized } from '../services/customer'
	import { onMount } from 'svelte'
	import { push } from '../router'

	setCallbackToFireWhenUnauthorized(() => {
		push(`/`);
	});

	function checkCookie(name) {
		if (document.cookie !== "") {
			var toCookie = document.cookie.split("; ")
			for (var i=0; i<toCookie.length; i++) {
				var CookieName = toCookie[i].split("=")[0]
				var CookieValue = toCookie[i].split("=")[1]
				if (CookieName == name) return CookieValue
			}
		}

		return null
	}

	function setCookie(name, value, expire) {
		document.cookie = name + "=" + value + ((expire===null)?"":("; expires=" + expire.toGMTString())) + "; path=/";
	}

	let showCookieConset = false
	onMount(async () => {
		showCookieConset = checkCookie('kk_cookie_permision') === null && document.location !== '/policy'
	});

	function acceptCookiePerm() {
		showCookieConset = false
		var expire = new Date();
		expire.setMonth(expire.getMonth() + 3);
		setCookie('kk_cookie_permision', 'yes', expire)
	}
</script>

{#if showCookieConset}
	<div  class="cookie-info d-flex flex-column">
		<div class="d-flex justify-content-between flex-grow-1 m-2">
			<div class="text-center flex-grow-1">
					Szanujemy Twoją prywatność<br />
					Na potrzeby naszej witryny korzystamy z plików cookie w celu personalizacji
					treści i reklam, udostępniania funkcji mediów społecznościowych oraz
					analizowania ruchu na stronie.
					<p>
						Pozwala nam to zapewnić maksymalną wygodę przy korzystaniu z
						naszych serwisów poprzez zapamiętanie Twoich preferencji i
						ustawień na naszych stronach.<br />
						Więcej informacji o plikach cookies i ich funkcjonowaniu znajdziesz
						w naszej
						<a href="#/policy" target="_blank">Polityce prywatności</a>.<br />
						Pamiętaj, że możesz zmienić ustawienia przeglądarki w zakresie korzystania
						z plików cookies.
					</p>
			</div>
			<div class="d-flex  align-items-center">
				<button class="accept-button" on:click={acceptCookiePerm}>AKCEPTUJĘ</button>
			</div>
		</div>
	</div>
{/if}

<style>
.cookie-info {
    display: -ms-flexbox;
    display: flex;
    position: fixed;
    bottom: 0;
    background: #ffffff;
    box-shadow: 0px -1px 4px -3px #000;
    width: 100%;
    z-index: 50;
    min-height: 112px;
}

.accept-button {
    height: auto;
    font-size: 16px;
    width: auto;
    padding: 8px;
    line-height: 1;
    background: #e5202e;
    color: white;
    margin-right: 65px;
}
</style>