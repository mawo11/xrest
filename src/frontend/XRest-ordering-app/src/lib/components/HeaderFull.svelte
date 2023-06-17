<script>
	import UserProfile from './UserProfile.svelte'
	import LanguageSwitcher from './LanguageSwitcher.svelte'
	import { _ } from '../services/localization/i18n'
</script>

<nav class="navbar navbar-expand-lg navbar-light bg-light shadow-lg py-3 border-bottom">
	<div class="container-fluid">
		<button
			class="navbar-toggler"
			type="button"
			data-bs-toggle="collapse"
			data-bs-target="#navbarSupportedContent"
			aria-controls="navbarSupportedContent"
			aria-expanded="false"
			aria-label="Toggle navigation">
			<span class="fas fa-ellipsis-v" />
		</button>
		<div class="col-md-4 text-end">
			<a href="/"><img
				src="/assets/img/logo_small.png"
				class="small-logo"
				alt="small logo"
			/></a>
		</div>
		<LanguageSwitcher/>
		<div class="collapse navbar-collapse" id="navbarSupportedContent">
			
			<ul class="navbar-nav me-auto ms-auto mb-2 mb-lg-0">
				<li>
					<a href="/"
						class="nav-link px-2 link-success text-uppercase fw-bold text-success"id="active">{$_('links.order')}</a>
				</li>
		
			</ul>

			<div class="col-md-3 text-start">
				<UserProfile/>
			</div>
		</div>
	</div>
</nav>