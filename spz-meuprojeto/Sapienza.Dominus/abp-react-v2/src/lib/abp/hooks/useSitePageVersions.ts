import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSitePageVersionsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  sitePageId?: string;
  }

export function useSitePageVersions(input: GetSitePageVersionsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["sitePageVersions", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/site-page-version", {
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

export function useAllSitePageVersions() {
  return useQuery({
    queryKey: ["sitePageVersions", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/site-page-version", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSitePageVersion(id: string) {
  return useQuery({
    queryKey: ["sitePageVersion", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/site-page-version/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSitePageVersion() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/site-page-version", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["sitePageVersions"] });
    },
  });
}

export function useUpdateSitePageVersion() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/site-page-version/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["sitePageVersions"] });
      queryClient.invalidateQueries({ queryKey: ["sitePageVersion", data.id] });
    },
  });
}

export function useDeleteSitePageVersion() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/site-page-version/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["sitePageVersions"] });
    },
  });
}


