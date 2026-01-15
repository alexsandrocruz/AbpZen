import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadLandingPagesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadWorkflowId?: string;
  }

export function useLeadLandingPages(input: GetLeadLandingPagesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadLandingPages", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-landing-page", {
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

export function useAllLeadLandingPages() {
  return useQuery({
    queryKey: ["leadLandingPages", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-landing-page", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadLandingPage(id: string) {
  return useQuery({
    queryKey: ["leadLandingPage", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-landing-page/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadLandingPage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-landing-page", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadLandingPages"] });
    },
  });
}

export function useUpdateLeadLandingPage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-landing-page/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadLandingPages"] });
      queryClient.invalidateQueries({ queryKey: ["leadLandingPage", data.id] });
    },
  });
}

export function useDeleteLeadLandingPage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-landing-page/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadLandingPages"] });
    },
  });
}


