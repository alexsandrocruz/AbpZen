import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSiteVisitEventsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  siteId?: string;
  }

export function useSiteVisitEvents(input: GetSiteVisitEventsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["siteVisitEvents", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/site-visit-event", {
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

export function useAllSiteVisitEvents() {
  return useQuery({
    queryKey: ["siteVisitEvents", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/site-visit-event", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSiteVisitEvent(id: string) {
  return useQuery({
    queryKey: ["siteVisitEvent", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/site-visit-event/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSiteVisitEvent() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/site-visit-event", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["siteVisitEvents"] });
    },
  });
}

export function useUpdateSiteVisitEvent() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/site-visit-event/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["siteVisitEvents"] });
      queryClient.invalidateQueries({ queryKey: ["siteVisitEvent", data.id] });
    },
  });
}

export function useDeleteSiteVisitEvent() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/site-visit-event/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["siteVisitEvents"] });
    },
  });
}


