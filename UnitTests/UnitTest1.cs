using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using THEMusicPlayer;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task CakeLyricsTest()
        {
            string lyrics = await Lyrics.Get("Cake", "Dime");
            Assert.AreEqual(DimeLyrics, lyrics);
        }

        [TestMethod]
        public async Task ThunderstruckLyricsTest()
        {
            //For reasons best known to itself, lyrics.wikia.com returns a 404
            //error code for this lyrics page but also renders valid HTML. Weird.
            string lyrics = await Lyrics.Get("AC/DC", "Thunderstruck");
            Assert.AreEqual(ThunderstruckLyrics, lyrics);
        }

        [TestMethod]
        public async Task AudienceOfOneLyricsTest()
        {
            string lyrics = await Lyrics.Get("Rise Against", "Audience Of One");
            Assert.AreEqual(AudienceOfOneLyrics, lyrics);
        }

        [TestMethod]
        public async Task NotFoundLyricsTest()
        {
            string lyrics = await Lyrics.Get("foo", "bar");
            Assert.AreEqual("Couldn't find lyrics for this track.", lyrics);
        }





        static string DimeLyrics =
            @"In the brown shag carpet of a cheap motel
In the dark and dusty corner by the TV shelf
Is a small reminder of a simpler time
When a crumpled up pair of trousers lost a brand new dime

Well you ask me how I made it through and how my mint condition could belong to you
When I'm on the ground I roll through town
I'm a president you don't remember getting kicked around

I'm a dime, I'm fine
And I shine I'm freshly minted
I am determined not to be dented
By a car or by a plane or anything not yet invented...

I'm a dime, I'm fine
And I shine

In the hiss and rumble of the freeway sounds
As the afternoon commuters drive their cars around
There's a ringle jingle near the underpass
There's a sparkle near the fast food garbage and roadside trash

I'm a dime, I'm fine
And I shine I'm freshly minted
I'm silver-plated, I'm underrated
You won't even pick me up because I'm not enough for a local phone call...

I'm a dime, I'm fine
And I shine
I'm a dime, I'm fine
And I shine
I'm a dime, I'm fine
And I shine";

        static string ThunderstruckLyrics = 
            @"(Thunder)
(Thunder)
(Thunder)
(Thunder)
(Thunder)
(Thunder)
(Thunder)
(Thunder)
(Thunder)
(Thunder)

I was caught
In the middle of a railroad track (Thunder)
Looked around
And I knew there was no turning back (Thunder)
My mind raced
And I thought, ""What can I do?"" (Thunder)
And I knew
There was no help, no help from you (Thunder)

Sound of the drums
Beatin' in my heart
The thunder of guns
Tore me apart

You've been thunderstruck

Went down the highway
Broke the limit, we hit the town
Went through to Texas, yeah, Texas
And we had some fun
We met some girls
Some dancers who gave a good time
Broke all the rules, played all the fools
Yeah, yeah, they, they, they blew our minds

And I was shakin' at the knees
Could I come again please?
Yeah, the ladies were too kind

You've been thunderstruck
Thunderstruck
Yeah yeah yeah, thunderstruck
Ooh, thunderstruck
Yeah

Now we're shaking at the knees
Could I come again please?

Thunderstruck
Thunderstruck
Yeah yeah yeah, thunderstruck
Thunderstruck, yeah yeah yeah

Said yeah, it's alright
We're doing fine
Yeah, it's alright
We're doing fine, so fine

Thunderstruck, yeah yeah yeah
Thunderstruck, thunderstruck
Thunderstruck
Whoa baby baby, thunderstruck
You've been thunderstruck
Thunderstruck
Thunderstruck
Thunderstruck
You've been thunderstruck";

        static string AudienceOfOneLyrics =
            @"I can still remember
The words and what they meant
As we etched them with our fingers
In years of wet cement

The days blurred into each other
Though everything seemed clear
We cruised along at half speed
But then we shifted gears

We ran like vampires from a thousand burning suns
But even then, we should have stayed

But we ran away
Now all my friends are gone
Maybe we've outgrown all the things that we once loved
Run away
But what are we running from?
A show of hands from those in this audience of one
Where have they gone?

Identities assume us
As nine and five add up
Synchronizing watches
With the seconds that we lost
And I looked up and saw you
I know that you saw me
We froze but for a moment
In empathy

I brought down the sky for you, but all you did was shrug
You gave my emptiness a name

And you ran away
Now all my friends are gone
Maybe we've outgrown all the things that we once loved
Run away
But what are we running from?
A show of hands from those in this audience of one
Where have they gone?

We're all OK, until the day we're not
The surface shines, while the inside rots
We raced the sunset and we almost won
We slammed the brakes, but the wheels went on

And we ran away
Now all my friends gone
Maybe we've outgrown all the things that we once loved
Run away
But what are we running from?
A show of hands from those in this audience of one
Where have they gone?";
    }
}
