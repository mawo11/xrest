<script>
	import HeaderFull from '../../lib/components/HeaderFull.svelte'
	import { _ } from '../../lib/services/localization/i18n'
	import { selectedRestaurant } from '../../lib/stores'

	$: document.title = $_("pageTitle") + ' - ' +  $selectedRestaurant.name 
</script>

<HeaderFull />
<div class="container-fluid">
	<slot />
</div>