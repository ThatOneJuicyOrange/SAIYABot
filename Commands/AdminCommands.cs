﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Driver;
using SAIYA.Creatures;
using SAIYA.Models;
using SAIYA.Systems;

namespace SAIYA.Commands
{
    public class AdminCommands : BaseCommandModule
    {
        [Command("testcreature")]
        public async Task TestCreature(CommandContext ctx)
        {
            if (ctx.User.Id != 283182274474672128) return;

            Creature creature = CreatureLoader.creatures[0];

            using (var fs = new FileStream(creature.CreatureTexture, FileMode.Open, FileAccess.Read))
            {
                DiscordMessageBuilder builder = new DiscordMessageBuilder();
                builder.AddFile($"image.png", fs);
                builder.AddEmbed(new DiscordEmbedBuilder()
                {
                    Title = $"{creature.Name}",
                    Description = $"{creature.Description}",
                    ImageUrl = "attachment://image.png"
                });
                await ctx.Channel.SendMessageAsync(builder);
            }
        }

        [Command("getegg")]
        public async Task GiveEgg(CommandContext ctx)
        {
            if (ctx.User.Id != 283182274474672128) return;
            var cmdUser = await User.GetOrCreateUser(ctx.User.Id, ctx.Guild.Id);
            var users = Bot.Database.GetCollection<User>("SAIYA_USERS");
            var update = Builders<User>.Update.Push(x => x.Eggs, new DatabaseEgg { Name = "Bleap", DateObtained = DateTime.Now.AddDays(-1) });
            await users.UpdateOneAsync(user => user.UserID == cmdUser.UserID && user.GuildID == cmdUser.GuildID, update);

            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("👍"));
        }
        [Command("potion")]
        public async Task Potion(CommandContext ctx)
        {
            if (ctx.User.Id != 283182274474672128) return;

            int test = 0;
            DiscordMessage msg = await ctx.Message.Channel.SendMessageAsync("test " + test);
            PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            while (await timer.WaitForNextTickAsync())
            {
                test++;
                await msg.ModifyAsync("test " + test);
                if (test > 10) return;
            }
            timer.Dispose();
        }
    }
}