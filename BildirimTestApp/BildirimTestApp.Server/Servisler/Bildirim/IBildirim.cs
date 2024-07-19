namespace BildirimTestApp.Server.Servisler.Bildirim;

public interface IBildirimKok { }

public interface IBildirim : IBildirimKok { }

public interface IBildirim<out TResponse> : IBildirimKok { }

public interface IAnlikBildirimKok { }

public interface IAnlikBildirim : IBildirim, IAnlikBildirimKok { }

public interface IAnlikBildirim<out TResponse> : IBildirim<TResponse>, IAnlikBildirimKok { }

public interface IEPostaBildirimKok { }

public interface IEPostaBildirim : IBildirim, IEPostaBildirimKok { }

public interface IEPostaBildirim<out TResponse> : IBildirim<TResponse>, IEPostaBildirimKok { }

public interface IDuyuruBildirimKok { }

public interface IDuyuruBildirim : IBildirim, IDuyuruBildirimKok { }

public interface IDuyuruBildirim<out TResponse> : IBildirim<TResponse>, IDuyuruBildirimKok { }
