<script>
	// Låtsasdata för att snabbt kunna bygga upp layouten
	const posts = [
	{
	id: 1,
	author: "Systemet",
	time: "Just nu",
	content: "MÅL! Yaqub Finey bryter dödläget i 82:a minuten för GIF Sundsvall. Målet bekräftat via VAR." // [cite: 39]
	},
	{
	id: 2,
	author: "Kuban1908",
	time: "1 min sedan",
	content: "Domaren måste ha tappat det totalt! Hur lång tid tog den där VAR-granskningen egentligen?"
	},
	{
	id: 3,
	author: "Svampen-Kungen",
	time: "5 min sedan",
	content: "Kom igen nu ÖSK! Vi måste trycka upp laget och vända det här."
	}
	];
</script>

<div class="max-w-2xl mx-auto bg-gray-100 min-h-screen">
	<header class="bg-slate-900 text-white p-4 sticky top-0 z-10 shadow-md">
		<h1 class="text-2xl font-black tracking-wider text-center uppercase">Läktaren</h1>
	</header>

	<main class="p-4 space-y-6">
		<section class="bg-white p-4 rounded-xl shadow-sm border border-gray-200">
			<textarea
                class="w-full p-3 bg-gray-50 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-900 focus:bg-white resize-none"
                rows="3"
                placeholder="Vad skriker du från läktaren just nu?"></textarea>
			<div class="flex justify-end mt-3">
				<button class="bg-slate-900 text-white px-6 py-2 rounded-lg font-bold hover:bg-slate-800 transition-colors">
					Vråla
				</button>
			</div>
		</section>

		<hr class="border-gray-300" />

		<section class="space-y-4">
			{#each posts as post (post.id)}
			<article class="bg-white p-4 rounded-xl shadow-sm border border-gray-200">
				<div class="flex justify-between items-baseline mb-2 border-b border-gray-100 pb-2">
					<span class="font-bold text-slate-900">{post.author}</span>
					<span class="text-xs text-gray-500 font-medium">{post.time}</span>
				</div>
				<p class="text-gray-800 leading-relaxed">{post.content}</p>
			</article>
			{/each}
		</section>
	</main>
</div>