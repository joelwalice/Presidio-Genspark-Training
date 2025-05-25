using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp.Classes
{
    public class Posts
    {
        public string Caption { get; set; }
        public int likes { get; set; }
            public static int GetValidUserNumbers()
            {
                int Users;
                Console.Write("Please enter the number of users: ");    
                if (int.TryParse(Console.ReadLine(), out Users) || Users < 0)
                {
                    Console.WriteLine("Please enter a valid number greater than 0");
                }
                return Users;
            }
            public static int GetValidPostCount()
            {
                int postCount;
                while (!int.TryParse(Console.ReadLine(), out postCount) || postCount < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number of posts (0 or more):");
                }
                return postCount;
            }
            public static int GetValidLikes()
            {
                int likes;
                while (!int.TryParse(Console.ReadLine(), out likes) || likes < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number of likes (0 or more):");
                }
                return likes;
            }

            public static string GetValidCaption()
            {
                string caption;
                do
                {
                    caption = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(caption))
                    {
                        Console.WriteLine("Invalid caption. Please enter a non-empty caption:");
                    }
                } while (string.IsNullOrWhiteSpace(caption));
                return caption;
            }
            static Posts[][] GetUserPosts(int userCount)
            {
                Posts[][] userPosts = new Posts[userCount][];

                for (int i = 0; i < userCount; i++)
                {
                    Console.Write($"\nUser {i + 1}: How many posts? ");
                    int postCount = GetValidPostCount();
                    userPosts[i] = new Posts[postCount];

                    for (int j = 0; j < postCount; j++)
                    {
                        Console.Write($"Enter caption for post {j + 1}: ");
                        string caption = GetValidCaption();

                        Console.Write("Enter likes: ");
                        int likes = GetValidLikes();

                        userPosts[i][j] = new Posts
                        {
                            Caption = caption,
                            likes = likes
                        };
                    }
                }

                return userPosts;
            }

            static void DisplayUserPosts(Posts[][] userPosts)
            {
                Console.WriteLine("\n------ Displaying Instagram Posts ------");
                for (int i = 0; i < userPosts.Length; i++)
                {
                    Console.WriteLine($"User {i + 1}:");

                    if (userPosts[i].Length == 0)
                    {
                        Console.WriteLine("No posts available.");
                    }
                    else
                    {
                        for (int j = 0; j < userPosts[i].Length; j++)
                        {
                            var post = userPosts[i][j];
                            Console.WriteLine($"Post {j + 1} - Caption: {post.Caption} | Likes: {post.likes}");
                        }
                    }

                    Console.WriteLine();
                }
            }

            public static void Run()
            {
                Console.Write("Enter number of users: ");
                int userCount = GetValidUserNumbers();

                Posts[][] userPosts = GetUserPosts(userCount);
                DisplayUserPosts(userPosts);
            }
        }
        
    }
