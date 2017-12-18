namespace Controls.Notification {
  public static class Toaster {

    public static ToastList Instance { get; set; }

    public static void NewToast(ToastProperties pProperties) {
      if (Instance == null) return;
      Instance.ToastPropertieses.Add(pProperties);
    }

  }
}
