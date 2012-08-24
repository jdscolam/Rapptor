namespace Rapptor.Domain.Authorization
{

	//	   stream: read a user's stream
	//• email: access a user's email address
	//• write_post: create a new post as a user
	//• follow: add or remove follows (or mutes) for this user
	//• messages: send and receive private messages as this user
	//• export:

	public class Scope
	{
		private Scope (string name)
		{
			Name = name;
		}

		public string Name { get; private set; }

		public static Scope Stream { get { return new Scope("stream"); } }
		public static Scope Email { get { return new Scope("email"); } }
		public static Scope WritePost { get { return new Scope("write_post"); } }
		public static Scope Follow { get { return new Scope("follow"); } }
		public static Scope Messages { get { return new Scope("messages"); } }
		public static Scope Export { get { return new Scope("export"); } }
	}
}