<script>
	import { _ } from '../services/localization/i18n'
	import UserProfile from './UserProfile.svelte'
	import LanguageSwitcher from './LanguageSwitcher.svelte'

</script>

<nav class="navbar navbar-expand-lg navbar-light bg-light shadow-lg  border-bottom">
	<div class="container-fluid">
	  
	  <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
		<!-- <span class="navbar-toggler-icon"></span> -->
		<span class="fas fa-ellipsis-v"></span>
	  </button>
	  <LanguageSwitcher/>
	  <div class="collapse navbar-collapse" id="navbarSupportedContent">

		<ul class="navbar-nav me-auto mb-2 mb-lg-0 ms-auto">
			<li><a href="/" class="nav-link px-2 link-success text-uppercase fw-bold text-success" id="active">{$_('links.order')}</a></li>


		</ul>

		<div class="col-md-3 text-start">
			<UserProfile />
		</div>

	  </div>

	</div>
  </nav>
<style>
	.navbar-brand { 
		margin: 0 !important;
	}
</style>