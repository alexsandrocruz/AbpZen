import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSitePagesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  siteId?: string;
  }

export function useSitePages(input: GetSitePagesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["sitePages", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/site-page", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllSitePages() {
  return useQuery({
    queryKey: ["sitePages", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/site-page", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSitePage(id: string) {
  return useQuery({
    queryKey: ["sitePage", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/site-page/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSitePage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/site-page", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["sitePages"] });
    },
  });
}

export function useUpdateSitePage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/site-page/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["sitePages"] });
      queryClient.invalidateQueries({ queryKey: ["sitePage", data.id] });
    },
  });
}

export function useDeleteSitePage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/site-page/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["sitePages"] });
    },
  });
}


